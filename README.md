[![LeadingRepository](https://img.shields.io/badge/Leading_Repository-Portal-blue)](https://github.com/eclipse-tractusx/portal)

# Portal Backend

This repository contains the forked and modified backend code for the Portal written in C#.

The original Portal application consists of

- [portal-frontend](https://github.com/eclipse-tractusx/portal-frontend),
- [portal-frontend-registration](https://github.com/eclipse-tractusx/portal-frontend-registration),
- [portal-assets](https://github.com/eclipse-tractusx/portal-assets) and
- [portal-backend](https://github.com/eclipse-tractusx/portal-backend).

The helm chart for installing the Portal is available in the [portal](https://github.com/eclipse-tractusx/portal) repository.

Please refer to the `docs` directory of the [portal-assets](https://github.com/eclipse-tractusx/portal-assets) repository for the overarching user and developer documentation of the Portal application.

The Portal is designed to work with the [IAM](https://github.com/eclipse-tractusx/portal-iam).

## Setup Instructions

`Updated setup instructions for your implementation`

Detail instruction to set up the applications can be found in this [Wiki](https://dsaas-tvs.atlassian.net/wiki/external/ZjIwOTYwZGYzNDY1NDEyMTliMGJiZDFjZjUyZGVjZTU). Use the following forked repos: 
- [Forked portal-backend](https://github.com/ProsonulHaque/portal-backend)
- [Forked portal-frontend](https://github.com/ProsonulHaque/portal-frontend)

For portal-assets repo, use the [original](https://github.com/eclipse-tractusx/portal-assets) one.

## API Documentation
`Swagger/OpenAPI documentation for your new endpoints`

The following APIs are implemented to manage company branding assets. Please run the project locally and go to [Swagger URL](http://localhost:5000/api/administration/swagger/index.html)

![Company Branding APIs](<Screenshot 2025-10-02 184259.png>)

## Authorization Discovery Report

`Explain how you researched and understood the role system`
- I started the investigation by inspecting the decoded JWT token of **Operator CX Admin** user. 
- Roles found in the token are not present in the database **user_roles** table. So, they must be set from the token provider.
- Then I investigated the keycloak IDPs (central and shared).
- After thorough investigation of the IDP consoles and KeyCloak documentation, I figured out that roles are mapped to users from the IDP console.
- The documentation also revealed that the clients in the IDP represent an application.
- **Cl2-CX-Portal** is the client id of the **portal-frontend** application.
- All user roles seem independent. I didn't find any direct dependency between **CX Admin** and **manage_branding_assets** roles.
- **OPERATOR** is a company role which is present in the **company_roles** table but not in the IDP. **Operator** role is present in the **user_role_collections** table but I couldn't make sense of this table. I'm not sure why this role is present in two different tables.

## Implementation Summary
`Describe your approach to mapping Operator - CX Admin to the manage_branding_assets role.`

I've mapped the **Operator CX Admin** user to the **manage_branding_assets** role through the following steps:

- Log into central IDP console
- Go to Users > cx-operator@tx.org > Role Mapping > Assign Role.
- The **manage_branding_assets** role was already added to the cx-operator@tx.org user. So, I didn't have to add the role.

I've added a manual check in the code that ensures only users with the **OPERATOR** company role can acces the resources.

I've followed the existing patterns in the project. Also, tried to follow clean code principles and REST constraints.

## Testing Results

`Demonstrate that your authorisation works correctly (Operators can access, non-Operators cannot)`

- I've tested the APIs with the **Operator CX Admin** (Operator) user and **Company A Admin**  (Non operator) user from Swagger. The APIs respond properly as expected. Operator can create, update and delete branding assets, non operator can't.
- Anyone can read branding assets.

## Problem-Solving Process

`Document the challenges you encountered and how you solved them`

- It took some time to understand the authorization process as KeyCloak was new for me. 
- Thanks to the detail documentaion from TVS which made the initial setup process very straight forward. 
- However I did some study about docker before using it. 
- I got stuck for a moment when I run **yarn build** command as it didn't work. Then I figured out I would have to use git bash terminal to use this script.
- I read the codes thoroughly to understand the coding patterns of the repository and the overall system architecture before adding any new code. 
- I introduced new codes only when I figured out how APIs are written in the project. I've tried to follow the existing pattern.

## Assumptions/Limitations
`Any assumptions or limitations you encountered`

- I've assumed that the **OPERATOR** role is not directly related to **CX Admin** role or **manage_branding_assets** role. In other systems, there are claims associated with a role (say, **CX Admin** role has **manage_branding_assets** claim along with other claims) but in this system I didn't find such a role-claim mapping. Rather all the roles of a client are independent. Maybe I've missed something and I need more investigation. 
- I've added a manual checking for company **OPERATOR** role.

## Known Issues and Limitations

See [Known Knowns](/CHANGELOG.md#known-knowns).

## Notice for Docker image

This application provides container images for demonstration purposes.

See Docker notice files for more information:

* [portal-registration-service](./docker/notice-registration-service.md)
* [portal-administration-service](./docker/notice-administration-service.md)
* [portal-marketplace-app-service](./docker/notice-marketplace-app-service.md)
* [portal-services-service](./docker/notice-services-service.md)
* [portal-notification-service](./docker/notice-notification-service.md)
* [portal-processes-worker](./docker/notice-processes-worker.md)
* [portal-portal-migrations](./docker/notice-portal-migrations.md)
* [portal-provisioning-migrations](./docker/notice-provisioning-migrations.md)
* [portal-maintenance-service](./docker/notice-maintenance-service.md)

## Notice for Nuget Packages

This application provides nuget packages to share functionalities across different repos. To see how the development and update of nuget packages is working please have a look at the [documentation](/docs/nuget/update-nuget-packages.md).

### Nuget

* [Org.Eclipse.TractusX.Portal.Backend.Framework.Async](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Async/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Cors](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Cors/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.DateTimeProvider](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.DateTimeProvider/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.DBAccess](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.DBAccess/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.DependencyInjection](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.DependencyInjection/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling](Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Controller](Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Controller)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Web](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Web/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.HttpClientExtensions](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.HttpClientExtensions/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Identity](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Identity/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.IO](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.IO/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Linq](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Linq/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Logging](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Logging/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Models](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Models/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Library](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Library/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Library.Concrete](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Library.Concrete/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.ProcessIdentity](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.ProcessIdentity/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Worker.Library](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Processes.Worker.Library/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Seeding/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Swagger](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Swagger/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Token](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Token/)
* [Org.Eclipse.TractusX.Portal.Backend.Framework.Web](https://www.nuget.org/packages/Org.Eclipse.TractusX.Portal.Backend.Framework.Web/)

## License

Distributed under the Apache 2.0 License.
See [LICENSE](./LICENSE) for more information.
