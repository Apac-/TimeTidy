# TimeTidy

Minimalist, browser based, remote client time-sheet logger with geo location services. Work sites are meant to be set up via a desktop environment, while logging on to or off of the work site should be done with a mobile device.

## Getting Started

1. Update database to current migration using package manager console in VS
```
Update-Database -TargetMigration:0
```
2. Create account at MapBox and put your access token in Scripts>leaflet-basics.js (Replace "ENTER_MAPBOX_KEY_HERE")
```
https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=ENTER_MAPBOX_KEY_HERE
```

## Deployment

1. Add production database connection string to web.config transform used for production deployment.
- You can usually acquire a generated connection string from your host -
```
<connectionStrings>
  <add name="DefaultConnection" 
    connectionString="Data Source=[Ip address];Database=[Database name];Initial Catalog=[Catalog name];User ID=[Admin user];Password=[Admin password]" providerName="System.Data.SqlClient"
    xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
</connectionStrings>
```
2. Add fixed machine key to deployment web.config transform used for production deployment. 
(Generate key: http://www.developerfusion.com/tools/generatemachinekey/)
```
<system.web>
  <machineKey 
    validationKey="Validation key goes here,IsolateApps"
    decryptionKey="Decryption key goes here,IsolateApps" 
    validation="Validation type goes here" descryption="Decryption type goes here"
    xdt:Transform="Insert"/>
</system.web>
```
3. Bring production database up to current migration.
Get script from code first migration via VS console:
- Do not run in VS, just copy the generated code -
```
Update-Database -Script -SourceMigration:0
```
Copy generated sql query and paste into sql query on database. 

4. Remove or change default admin account. Current seeded admin account is:
```
Username: Admin@timetidy.com
Password: Admin@vigilance1
```


## Built With

* [AspNet.MVC](https://www.asp.net/mvc) - v5.2.3
* [AspNet.WebApi](https://www.asp.net/web-api) - v5.2.2
* [EntityFramework]() - v6.1.1 - ORM
* [Autofac](https://autofac.org/) - IoC container

* [Mapbox](http://www.mapbox.com/) - Map provider
* [Leaflet](http://leafletjs.com/) - Makes the maps interactive
* [Bootbox.js](http://bootboxjs.com/) - Used for easy alert boxes
* [Moment.js](https://momentjs.com/) - UTC time parsing
* [DataTables](https://datatables.net/) - Creates all the clean tables of data
* [Bootstrap](http://getbootstrap.com/javascript/) - Lumen theme

