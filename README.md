# Console tool for managing a database server

## 2 WINDOWS:
###LOGIN:
-> Enter username and password stored in database_api/Documents/user.txt; stored in plain to demonstrate filesystem input

###APP:
-> Input database commands through the console
-> Commands are listed in command_list.txt or use the 'help' command to display the full list

##Test command sequence:
```bash


```bash
dbcon localhost 5432 postgres postgres [AdminPassword]
```
```bash
crusr test_user->123456Ab
```
```bash
newdb test_db
```
```bash
dbdc
```
```bash
dbcon localhost 5432 test_db test_user 123456Ab
```
```bash
newtb student jmeno->varchar prijmeni->varchar vek->integer
```
```bash
insert student jmeno->st1->varchar prijmeni->st1p->varchar vek->99->integer
```
```bash
insert student jmeno->st2->varchar prijmeni->st2p->varchar vek->50->integer
```
```bash
insert student jmeno->st3->varchar prijmeni->st3p->varchar vek->75->integer
```
```bash
select student
```
```bash
update student prijmeni->st1p->varchar jmeno->jmeno_n->varchar
```
```bash
select student
```
```bash
delete student jmeno->st1->varchar
```
```bash
select student
```
```bash
dbdc
```
```bash
logout
```
