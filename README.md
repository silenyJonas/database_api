# Console tool for managing a PostgreSQL database

## 2 WINDOWS:
### LOGIN:
-> Enter username and password stored in database_api/Documents/user.txt; stored in plain to demonstrate filesystem input

### APP:
-> Input database commands through the console
-> Commands are listed in command_list.txt or use the 'help' command to display the full list
##
### Test command sequence:

login server
```bash
dbcon localhost 5432 postgres postgres [AdminPassword]
```
create user
```bash
crusr test_user->123456Ab
```
create database
```bash
newdb test_db
```
dissconnect database
```bash
dbdc
```
connect database
```bash
dbcon localhost 5432 test_db test_user 123456Ab
```
create table
```bash
newtb student jmeno->varchar prijmeni->varchar vek->integer
```
insert into table
```bash
insert student jmeno->st1->varchar prijmeni->st1p->varchar vek->99->integer
```
insert into table
```bash
insert student jmeno->st2->varchar prijmeni->st2p->varchar vek->50->integer
```
insert into table
```bash
insert student jmeno->st3->varchar prijmeni->st3p->varchar vek->75->integer
```
select 
```bash
select student
```
update
```bash
update student prijmeni->st1p->varchar jmeno->jmeno_n->varchar
```
select
```bash
select student
```
delete
```bash
delete student jmeno->st1->varchar
```
select
```bash
select student
```
dissconnect database
```bash
dbdc
```
logout app
```bash
logout
```
