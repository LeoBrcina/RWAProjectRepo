--Log Item
create table LogItem
(
IDLog int primary key identity,
Tmstamp datetime,
Lvl int,
Txt nvarchar(max)
)

--UserRole
create table UserRole
(
IDUserRole int primary key identity,
RoleName nvarchar(150)
)

--User
create table UserGamer
(
IDUserGamer int primary key identity,
FirstName nvarchar(150),
LastName nvarchar(150),
Email nvarchar(150),
City nvarchar(150),
PostalCode nvarchar(20),
HomeAddress nvarchar(150),
Username nvarchar(100),
PwdHash nvarchar(256),
PwdSalt nvarchar(256),
UserRoleID int foreign key references UserRole(IDUserRole)
)

--GameType 1-to-N Entity
create table GameType
(
IDGameType int primary key identity,
GameTypeName nvarchar(150),
[Description] nvarchar(150)
)

--Game Primary Entity
create table Game
(
IDGame int primary key identity,
GameName nvarchar(150),
GameTypeID int foreign key references GameType(IDGameType),
[Description] nvarchar(150),
Size int
)

--Review User M-to-N Bridge
create table Review
(
IDReview int primary key identity,
GameID int foreign key references Game(IDGame),
GamerID int foreign key references UserGamer(IDUserGamer),
Rating int,
ReviewText nvarchar(max)
)

--Genre M-to-N Entity
create table Genre
(
IDGenre int primary key identity,
GenreName nvarchar(150),
[Description] nvarchar(150)
)

--GameGenre M-to-N Bridge
create table GameGenre
(
IDGameGenre int primary key identity,
GameID int foreign key references Game(IDGame),
GenreID int foreign key references Genre(IDGenre)
)

insert into GameType values ('Singleplayer', 'A game for one player.')
insert into GameType values ('Multiplayer', 'A game for more players.')
insert into GameType values ('FPS', 'First Person Shooter.')
insert into GameType values ('TPS', 'Third Person Shooter.')

insert into Game values ('Uncharted 2', 4, 'Greatest of all time.', 15)
insert into Game values ('GTA V', 4, 'The game that made the difference.', 75)
insert into Game values ('Withcer 3', 4, 'Eversone loves this one.', 55)
insert into Game values ('The Last of Us', 4, 'Most awards ever.', 45)
insert into Game values ('Call of Duty', 3, 'Best FPS.', 115)
insert into Game values ('Battlefield', 3, 'Big battles.', 95)
insert into Game values ('Minecraft', 2, 'Cubes.', 15)
insert into Game values ('HALO', 3, 'XBOX GOAT.', 75)
insert into Game values ('Formula 1', 2, 'Lewis Hamilton.', 55)
insert into Game values ('Need for Speed', 2, 'Most Wanted.', 30)

insert into UserRole values ('Admin')
insert into UserRole values ('User')

insert into Genre values ('Action', 'For those that like explosions.')
insert into Genre values ('Adventure', 'For those that like an adventure.')
insert into Genre values ('Horror', 'For those that dont scare easily.')
insert into Genre values ('RPG', 'For those that like to role play.')

insert into GameGenre values (1, 1)
insert into GameGenre values (1, 2)
insert into GameGenre values (2, 1)
insert into GameGenre values (2, 2)
insert into GameGenre values (3, 1)
insert into GameGenre values (3, 2)
insert into GameGenre values (3, 3)
insert into GameGenre values (4, 1)
insert into GameGenre values (4, 2)
insert into GameGenre values (4, 3)
insert into GameGenre values (5, 1)
insert into GameGenre values (6, 1)
insert into GameGenre values (7, 2)
insert into GameGenre values (7, 4)
insert into GameGenre values (8, 1)
insert into GameGenre values (8, 2)
insert into GameGenre values (9, 1)
insert into GameGenre values (10, 1)
