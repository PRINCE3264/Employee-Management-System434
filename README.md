🏢 Title:

Employee Management System — ASP.NET Core Web API (JWT Authentication) + Angular

📝Description:

This project is a full-stack Employee Management System built with ASP.NET Core Web API (backend) and Angular (frontend).
It includes secure JWT-based authentication and role-based access control for Admin and Employee users.

🔑 Key Features:

Authentication: Login & Register using ASP.NET Core Identity with JWT tokens

Role Management: Admin and Employee roles with protected routes

Departments: Add, Edit, Delete, and View department details

Employees: CRUD operations with linked user accounts and profile management

Leave Management:

Employees can apply or cancel leave

Admins can approve or reject leave requests

Attendance Module: Employees can mark attendance; Admins can view attendance reports

Angular Frontend: Responsive UI with Angular Material and Tailwind CSS

Secure API: Token-based authentication for all protected endpoints

# WebApp

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.2.5.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
