using FileData.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileData
{
    public class FileDataProcessor
    {
        public enum FileActions
        {
            Version,
            Size
        }
        private readonly string[] CommandLineArgs;
        private readonly ILogger logger;
        private const int actionArgLocation = 0;
        private const int filePathArgLocation = 1;
        private const int minimumRequiredArguments = 2;
        private const int maximumRequiredArguments = 2;

        public FileDataProcessor(string[] commandLineArgs)
        {
            CommandLineArgs = commandLineArgs;
            logger = Log.ForContext<FileDataProcessor>();
        }

        /// <summary>
        /// Validates the arguments passed in to constructor
        /// </summary>
        /// <param name="errorMessages"></param>
        /// <returns>A boolean indicating whether args are valid or not</returns>
        public bool HasValidArgs(out Collection<string> errorMessages)
        {
            errorMessages = new Collection<string>();

            // Check for correct number of arguments
            if (CommandLineArgs.Length < minimumRequiredArguments)
            {
                string message = "Too few arguments.";
                errorMessages.Add(message);
                logger.Debug($"Argument Validation Failed: {message}");

                if (CommandLineArgs.Length == 0)
                {
                    // No arguments, so return as nothing further to validate.
                    return false;
                }
            } 
            else if (CommandLineArgs.Length > maximumRequiredArguments)
            {
                string message = "Too many arguments.";
                errorMessages.Add(message);
                logger.Debug($"Argument Validation Failed: {message}");
            }

            // Check that we have a valid action to perform on the target file
            if (DetermineActionsBasedOnArgs().Count() < 1)
            {
                string message = $"{CommandLineArgs[actionArgLocation]} is an unknown action or command";
                errorMessages.Add(message);
                logger.Debug($"Argument Validation Failed: {message}");
            }

            // We could introduce some filepath validation here

            if (errorMessages.Count > 0)
            {
                return false;
            }

            logger.Debug("All arguments are valid");
            return true;
        }

        /// <summary>
        /// Runs the arguments against the target file.
        /// </summary>
        /// <returns>A List of results based on the actions performed on the file</returns>
        public List<string> RunActions()
        {
            // Get Full filepath
            string filePath = CommandLineArgs[filePathArgLocation];
            // Could introduce some filepath validation at this point

            logger.Debug($"Running file actions for target file: ${filePath}");

            // Determine actions to take based on args
            Collection<FileActions> actions = DetermineActionsBasedOnArgs();

            // iterate through actions and store results
            List<string> actionResults = new List<string>();
            foreach (var action in actions)
            {
                actionResults.Add(PerformAction(action, filePath));
            }

            logger.Debug($"No of actions run for target file: {actions.Count()}");

            // return result
            return actionResults;
        }

        /// <summary>
        /// Performs An individual action on the target filePath
        /// </summary>
        /// <param name="action"></param>
        /// <param name="filePath"></param>
        /// <returns>A string representation of the result of the action</returns>
        private string PerformAction(FileActions action, string filePath)
        {
            ArgHandlerFactory argHandlerFactory = new ArgHandlerFactory();
            var ArgHandler = argHandlerFactory.Create(action);

            logger.Debug($"Performing action: {action.ToString()} on target file: {filePath}");

            return ArgHandler.RunArgAction(filePath);
        }

        /// <summary>
        /// Determins the actions to be taken on the target file based on passed in arguments
        /// </summary>
        /// <returns></returns>
        private Collection<FileActions> DetermineActionsBasedOnArgs()
        {
            /*
             * Some future proofing introduced here incase we should ever want to allow
             * more than one file action to be taken on a file. i.e. command line args -v -s c:\somtestfile.txt
             * could return both version and size for the file
             */

            Collection<FileActions> actions = new Collection<FileActions>();
            ArgHandlerFactory argHandlerFactory = new ArgHandlerFactory();

            var actionTypes = Enum.GetValues(typeof(FileActions)).Cast<FileActions>();
            foreach (var actionType in actionTypes)
            {
                var argHandler = argHandlerFactory.Create(actionType);
                if (argHandler.IsValidArg(CommandLineArgs[actionArgLocation]))
                {
                    actions.Add(actionType);
                }
            }

            return actions;
        }
    }
}
