konzolový nástroj pro správu db serveru
2 OKNA:
LOGIN:
->zadat jméno a heslo ulozené v database_api/Documents/user.txt
APP:
->input do db skrze pøíkazy v konzoly -prikazy v command_list.txt nebo vypsat cely list prikazem help

testovací sekvence pøíkazù:

dbcon localhost 5432 postgres postgres [HesloNastavenéUAdmin]
crusr test_user->123456Ab
newdb test_db
dbdc
dbcon localhost 5432 test_db test_user 123456Ab
newtb student jmeno->varchar prijmeni->varchar vek->integer
insert student jmeno->st1->varchar prijmeni->st1p->varchar vek->99->integer
insert student jmeno->st2->varchar prijmeni->st2p->varchar vek->50->integer
insert student jmeno->st3->varchar prijmeni->st3p->varchar vek->75->integer
select student
update student prijmeni->st1p->varchar jmeno->jmeno_n->varchar
select student
delete student jmeno->st1->varchar
select student
dbdc
logout