# CreateModule

## Description

 CreateModule is a small dev console utility that creates modules in a modular monolithic architecture.

## Getting Started

### Prerequisites

* No prerequisites

### Installation

1. Clone the repository: `git clone https://github.com/afaolek/CreateModule.git`
2. Restore: `dotnet restore`

## Usage

* After building, add the executable path to your Path environment variable.
* Then run the command `CreateModule moduleName --options` in the folder you want to create the module. There are only two options:
  * `--noTests` if you don't want to generate the Tests projects
  * `--mdc` if you want to merge the `Domain` and `Application` projects into one project. The project will be called `ApplicationCore`.

## Contributing

* Fork the repository and create a new branch
* Make changes and commit them with a descriptive commit message
* Create a pull request to merge your changes

## License

* MIT License

## Authors

* Adeleke Ademolu - [afaolek](https://github.com/afaolek)
