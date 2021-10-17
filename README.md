# Thor

## There are 4 paths for requests:
### /login
POST with "password = string" to login

### /logout
POST to logout

### /get_c
POST with 
{
  "type": uint,
  "format": uint,
  "size": uint,
  "numberOfItems": uint
}
for a random string according to the type, format etc.
Please view CModel.cs for more details.

### /get_f
POST with "size = ulong" for a random binary file of such size

## Password
The password is in appsettings.json, you can change it as you wish.

## Controllers/AccountController
Please review the GenerateGetCItemListForResponse() method.
