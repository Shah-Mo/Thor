# Thor

## There are 4 paths for requests:
/login
POST with "password = string" to login

/logout
POST to logout

/get_c
GET for a mirror response

/get_f
POST with "size = ulong" for a random binary file of such size

## Password
The password is in appsettings.json, you can change it as you wish.

## Controllers/AccountController
Review the GetC() and GetF() methods, I left some comments.
