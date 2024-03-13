# Build and Test Workflow

This GitHub Actions workflow is designed to automate the process of building and testing a .NET solution. It utilizes the dotnet CLI tool for managing dependencies, building the solution, and running tests.

## Trigger

This workflow is triggered when it is called. It accepts an input parameter `dotnet-version`, which specifies the version of the dotnet CLI tool to use. If not provided, it defaults to version '8.0.x'.

## Jobs

### Build and Test

This job executes the following steps on an Ubuntu environment:

1. **Setup Dotnet**:
   - Uses the GitHub Actions action `setup-dotnet@v4` to set up the specified version of the dotnet CLI tool.
   - The version used is determined by the input parameter `dotnet-version`.

2. **Checkout Repository**:
   - Uses the GitHub Actions action `checkout@v4` to checkout the repository's source code.

3. **Restore Dependencies**:
   - Runs the `dotnet restore` command to restore the project dependencies.

4. **Build Solution**:
   - Runs the `dotnet build` command with the `--configuration Release` flag to build the solution in Release mode.

5. **Run Tests**:
   - Executes the `dotnet test` command with the `--no-build` flag and normal verbosity to run tests without rebuilding the solution.

Each step in this job is crucial for the continuous integration process, ensuring that the solution is built successfully and tests are run on each code change.
