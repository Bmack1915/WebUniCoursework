---- Simple RESTful backend Web API ------

https://github.com/Bmack1915/WebUniCoursework.git

This Web API has been created to demonstrate an understanding of a dotnet Web API, by creating five simple models represeneting entities in a Sports League.

Entites
--------
The following entities exist to model a simple sports league:

- Player
- Team
- Venue
- Match
- League

Each entity has a Controller to allow for basic CRUD operations, each with simple validation and console logging.
In addition to controllers for each entity, Role, Account and Data Controllers have been added to manage roles, account management and the autogeneration of some data points (should someone using the service wish to deploy their own blank database) respectively.

Steps to run the Web API
-------------------------
1. Clone this repository
2. Ensure the correct dependencies are restored (In Visual Studio)
3. Build the project
4. Run the project (which will be accessible at https://localhost:7220 by default) when run locally.

Deploying to Azure
-------------------
This Web API uses a local db (in this case a 'SportsLeague.db') which can be seen in the repo and within the solution tree in VS. When deploying to Azure through VS, it is crucial that the Build Proprties (right click on the SportsLeague.db, Properties -> Build) are set to 'EmbeddedResource' and "Always Copy", so that the db file is seen by Azure during the build.

webcourseworkuni.azurewebsites.net is the domain for this service, which can then be consumed in postman, provided it is running.

Endpoints
---------
The following endpoints exist for all controllers: 

- `GET /api/[controller]`: Get a list of all [correspondingModel]s.
- `GET /api/[controller]/{id}`: Get details of a specific [correspondingModel] by ID.
- `POST /api/[controller]`: Create a new [correspondingModel].
- `PUT /api/[controller]/{id}`: Update details of a [correspondingModel].
- `DELETE /api/[controller]/{id}`: Delete a [correspondingModel] by ID.

(Example)
-GET-
https://localhost:7220/api/player : GET a list of all players (CONSUME IN POSTMAN)
https://localhost:7220/api/player/1 : GET player with PlayerId 1 (CONSUME IN POSTMAN)

Authentication + Authorization
------------------------------

End points are protected by use of a JwT token. Once a token is received it should be passed into the Authorization header, using the 'Bearer token' selection.

To receive a token, someone is required to register using 'api/account/register' providing a valid Email and Password. This person will receive a link to verify their email.

By default, newly registered accounts to the system must be made a User by an Admin. For demstration purposes an Admin account credentials are provided below to allow for testing for the assessment of this coursework.

ADMIN CREDENTIALS.
{
    "Email" : "benmackwebdev@gmail.com",
    "Password" : "Password123!"
}

Once logged in an Admin token will be generated for use to add any newly registered account to the User role using '/api/roles/assign-role-to-user'.
Once any newly registered accounts are assigned the User role, they can login to recieve their JwT token to access endpoints requiring User authorization.
Most endpoints (aside from Account controller) require both User and Admin roles, however most POST, PUT and DELETE methods require Admin access.

Note: JwT tokens cannot be revoked, they just expire after a period of time (1 hour).

