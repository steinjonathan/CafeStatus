--create database StatusCafe
use StatusCafe

create table StatusLog 
(
	CodigoCafeStatus int PRIMARY KEY identity,
	Data datetime not null,
	Pronto bit not null default 0,
	Observacao varchar(255) null
)

select * from statuslog

