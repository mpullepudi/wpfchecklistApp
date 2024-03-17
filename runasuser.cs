using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        // Specify the username, domain, and password for the user you want to run the process as
        string username = "username";
        string domain = "domain";
        string password = "password";

        // Specify the path to the executable you want to run
        string executablePath = "C:\\path\\to\\your\\executable.exe";

        // Call the CreateProcessAsUser function to run the process as a different user
        bool success = CreateProcessAsUser(username, domain, password, executablePath);

        if (success)
        {
            Console.WriteLine("Process started successfully.");
        }
        else
        {
            Console.WriteLine("Failed to start the process.");
        }

        Console.ReadLine();
    }

    static bool CreateProcessAsUser(string username, string domain, string password, string executablePath)
    {
        IntPtr userTokenHandle = IntPtr.Zero;
        IntPtr primaryTokenHandle = IntPtr.Zero;

        // LogonUser function to obtain a handle to the specified user's access token
        bool result = LogonUser(username, domain, password, LogonType.LOGON32_LOGON_INTERACTIVE, LogonProvider.LOGON32_PROVIDER_DEFAULT, out userTokenHandle);
        if (!result)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // DuplicateTokenEx function to create a primary token
        result = DuplicateTokenEx(userTokenHandle, 0, IntPtr.Zero, SecurityImpersonationLevel.SecurityImpersonation, TokenType.TokenPrimary, out primaryTokenHandle);
        if (!result)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        // Create the process using the primary token
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = executablePath;
        startInfo.UseShellExecute = false;
        startInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(executablePath);
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;

        // Set the token for the new process
        startInfo.UserName = username;
        startInfo.Password = new System.Security.SecureString();
        foreach (char c in password)
        {
            startInfo.Password.AppendChar(c);
        }
        startInfo.Domain = domain;

        Process process = Process.Start(startInfo);

        return true;
    }

    // P/Invoke declarations

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, LogonType dwLogonType, LogonProvider dwLogonProvider, out IntPtr phToken);

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, SecurityImpersonationLevel ImpersonationLevel, TokenType TokenType, out IntPtr phNewToken);

    enum LogonType : int
    {
        LOGON32_LOGON_INTERACTIVE = 2,
        LOGON32_LOGON_NETWORK = 3,
        LOGON32_LOGON_BATCH = 4,
        LOGON32_LOGON_SERVICE = 5,
        LOGON32_LOGON_UNLOCK = 7,
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
        LOGON32_LOGON_NEW_CREDENTIALS = 9,
    }

    enum LogonProvider : int
    {
        LOGON32_PROVIDER_DEFAULT = 0,
        LOGON32_PROVIDER_WINNT50 = 3,
        LOGON32_PROVIDER_WINNT40 = 2,
        LOGON32_PROVIDER_WINNT35 = 1,
    }

    enum SecurityImpersonationLevel : int
    {
        SecurityAnonymous = 0,
        SecurityIdentification = 1,
        SecurityImpersonation = 2,
        SecurityDelegation = 3,
    }

    enum TokenType : int
    {
        TokenPrimary = 1,
        TokenImpersonation = 2,
    }
}
