# Card Manager

This application is part of a live coding to present some patterns and refactoring techniques that help make unit tests
easier to write and maintain as the application evolves.

> **Note:** This is a example application designed with the purpose of teaching. Some design decisions have been
> simplified for the sake of teaching.

The main work is presented in this [PR](https://github.com/leonardo-lorenzon/CardManager/pull/1). 
Each commit is a single pattern or refactoring technique. Take a look on each one comparing side-by-side with the previous code.

## Running locally

To run in watch mode:
```shell
dotnet watch --project src/CardManager.Api
```

Run Tests:
```shell
dotnet test
```

## References

1) xUnit Test Patterns: Refactoring Test Code - Gerard Meszaros
2) Working Effectively with Legacy Code - Michael Feathers
3) Growing Object-Oriented Software, Guided by Tests - Freeman, Pryce
4) Refactoring: Improving the Design of Existing Code - Martin Fowler
5) Clean Architecture: A Craftsman's Guide to Software Structure and Design  - Robert C. Martin
6) Domain-Driven Design: Tackling Complexity in the Heart of Software - Eric Evans
7) [Mocks Aren’t Stubs](https://martinfowler.com/articles/mocksArentStubs.html)
8) [Replacing Throwing Exceptions with Notification in Validations](https://martinfowler.com/articles/replaceThrowWithNotification.html)

