using System.IO;
using System.Threading;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Input;
using FlaUI.UIA3;

class Program
{
    static void Main(string[] args)
    {
        // Specify the path of the file
        string filePath = @"C:\Path\To\Your\File.txt";

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Create the automation object
            using (var automation = new UIA3Automation())
            {
                // Get the desktop automation element
                var desktop = automation.GetDesktop();

                // Open File Explorer
                Keyboard.TypeSimultaneously(VirtualKeyShort.LWIN, VirtualKeyShort.VK_E);
                Thread.Sleep(1000); // Wait for File Explorer to open

                // Focus on the address bar
                Keyboard.Type(VirtualKeyShort.ALT);
                Thread.Sleep(100);
                Keyboard.Type(VirtualKeyShort.D);
                Thread.Sleep(1000); // Wait for the address bar to be focused

                // Type the file path
                Keyboard.Type(filePath);
                Thread.Sleep(1000); // Wait for the path to be entered
                Keyboard.Type(VirtualKeyShort.RETURN);
                Thread.Sleep(2000); // Wait for File Explorer to navigate to the path

                // Get the file automation element
                var fileElement = desktop.FindFirstChild(cf => cf.ByValue(filePath));

                if (fileElement != null)
                {
                    // Right-click on the file
                    fileElement.RightClick();
                    Thread.Sleep(1000); // Wait for the context menu to appear

                    // Find and click on a specific menu item from the context menu
                    var menuItem = desktop.FindFirstDescendant(cf => cf.ByText("YourMenuItem"));

                    if (menuItem != null)
                    {
                        menuItem.Click();
                        Console.WriteLine("Menu item clicked successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Menu item not found.");
                    }
                }
                else
                {
                    Console.WriteLine("File not found in File Explorer.");
                }
            }
        }
        else
        {
            Console.WriteLine("File does not exist.");
        }
    }
}
