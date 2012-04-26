### What is this?
Rossie is a project that will allow you to run code snippets in a secure sandbox with very low trust level and get a result back.

The system uses the latest version of Roslyn from Microsoft ( http://www.microsoft.com/download/en/details.aspx?id=27746 ).

This is an example of how Rossie works:

1. Start the Rossie Engine Service
2. Connect to it with the Demo application
3. Run the following code snippet: return BitConverter.ToString(System.Text.Encoding.Default.GetBytes("me@home"));
4. See the following result: "6D-65-40-68-6F-6D-65"

This is not a standard REPL and Rossie does not keep any state.

You can read this (http://blog.filipekberg.se/2011/12/08/hosted-execution-of-smaller-code-snippets-with-roslyn/) blog post about how I created this.

### What is left to do?
* Add comments
* Break out minor parts of the compilation,execution and validation step.


### License
All included software is licensed under the MIT License available in full in the LICENSE file.