# Introduction 
This is a payment file processing service built with .NET 7.  It contains:
  - A .NET Worker service project
  - A xUnit Unit Test project

# Adding new payment file 

# Getting Started
##Follow these steps to get the pipeline created and configured:
1. Replace the Hello World app with your own project and update this readme
2. Replace the Hello World Test project with your own unit test project
3. Remove or add stages in `azure-pipelines.yml` to describe the app Lifecycle.  The devault stages are **Build > DEV > QA > TRN > PROD**
4. Update `variables-<env>.yml` files for each environment you need with the appropriate token values
5. Configure secret values and variables shared between multiple pipelines inside of your variable groups
    - **Pipelines > Library > core-entauto-dev** for environment specific values
    - **Pipelines > Library > core-entauto-shared** for values shared with all environments
6. Create a pull request to merge your code.  A build will run that includes
    - Build the Application
    - Unit Tests
    - Blackduck Scan
    - Sonarqube Scan
    - Checkmarx Scan
7. Ensure that you have VM Resource Agents tagged with your repo name in each Environment you wish to deploy. This may require coordination with BPS or the server owners
8. Complete the pull request to Build, Package and Deploy your application

# Build and Test
The pipeline is configured to Build and Test when a Pull Request is created and Build, Package and Release when the master branch is updated

# ConcurrentProcessor
