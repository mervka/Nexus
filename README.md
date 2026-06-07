# NEXUS

NEXUS is an ASP.NET Core MVC web application developed as a course project for Programming for the Internet.

The application allows users to share project ideas, explore active projects, apply to projects, and form teams after founder approval.

## Main Features

- User registration and login with ASP.NET Core Identity
- Editable user profiles with bio, skills, experience level, links, and profile image upload
- Profile completion requirement before applying to projects
- Project creation, editing, details, and deletion
- Explore page for active projects
- Project application system
- Duplicate application prevention
- Founder approval or rejection of applications
- Accepted applicants become project members
- Projects page showing the user's created and joined projects
- Teams page showing founders and accepted members
- Team member profile visibility after approval
- Basic authorization and secure CRUD operations

## Technologies Used

- ASP.NET Core MVC
- Razor Views
- Entity Framework Core
- ASP.NET Core Identity
- SQLite
- LINQ
- Bootstrap
- HTML / CSS
- Git / GitHub

## How to Run the Project

### 1. Clone the repository

```bash
git clone https://github.com/mervka/Nexus.git
cd Nexus
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

After running the project, open the localhost URL shown in the terminal.

## Demo Accounts

You can use the following demo accounts to test the application:

### Founder Account

```text
Email: merve@gmail.com
Password: Deneme12.
```

### Applicant / Team Member Account

```text
Email: eren@gmail.com
Password: Eren1525.
```

## Basic Test Flow

1. Log in with the founder account.
2. Create a new project from `Share an Idea`.
3. Log out and log in with the applicant account.
4. Go to `Explore Projects`.
5. Apply to an active project.
6. Log back in with the founder account.
7. Open the project from the `Projects` page.
8. View applications and accept or reject the applicant.
9. Log in again with the applicant account.
10. Check that the accepted project appears in the `Projects` and `Teams` pages.

## File Upload

The project includes profile image upload.

Uploaded profile images are stored under:

```text
wwwroot/uploads/profiles
```

The database stores only the image path in the `ProfileImagePath` field. The image file itself is not stored directly in the database.

## Project Status

This is an MVP version.

The following features are not included in this version:

- Real-time chat
- Notification system
- Admin panel
- Payment system
- Advanced matching system
- Task or milestone management
