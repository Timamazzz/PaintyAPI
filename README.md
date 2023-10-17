# PaintyAPI
Test task for a c# developer vacancy in Paintly

**Task:**
Implement a system for saving and viewing images. Users can upload images, view their own images, add friends who can view the user's images.

**Functional Requirements:**
- User registration is required.
- Users should be able to upload images.
- Users should be able to view their own images.
- Users should be able to view another user's images if they are friends.
- Users should be able to add another user as a friend (User A can view User B's images if User B has added User A as a friend, but User B cannot view User A's images unless User A reciprocates).

**Non-Functional Requirements:**
- The application should be developed using .NET 7, C# 11, and ASP.NET Core.
- Any form of authentication is acceptable (e.g., ASP.NET Identity or storing login/password in the database and using basic auth).
- Entity Framework Core should be used for implementation with a rich domain model.
- The user entity's image collection should be a private field, and only an IReadOnlyCollection property should be exposed to the outside (configuration of EF should be set up to enable this).
- Images themselves should be stored in a local storage, and the storage path should be configurable via appsettings.json. The application should continue to function correctly when moving the storage location for both images and the images themselves (no absolute paths should be stored in the database).
- The user-friend relationship (many-to-many on users) should be implemented using EF configuration.
- The application should have a RESTful API that returns appropriate error codes (404, 401, 403, etc.).
- The application should have Swagger with the ability to configure the chosen authentication method through the UI.
- The application should have all the necessary endpoints to implement the functional requirements.
- A front-end is optional; Swagger is encouraged.
- Other details are at your discretion.
