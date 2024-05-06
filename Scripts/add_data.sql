-- add user, roles, privileges
INSERT INTO Users (email, birthday, name, password, username) VALUES
('admin@gmail.com', '2004-05-21', 'Nguyen Huu Loc', '1h0ATANFe6x7kMHo1PURE74WI0ayevUwfK/+Ie+IWX/xWrFWngcVUwL/ewryn38EMVMQBFaNo4SaVwgXaBWnDw==', 
 'admin');

INSERT INTO Roles (name) VALUES ('ADMIN');
INSERT INTO Roles (name) VALUES ('USER');

INSERT INTO Privileges (name) VALUES ('READ');
INSERT INTO Privileges (name) VALUES ('WRITE');
INSERT INTO Privileges (name) VALUES ('DELETE');

INSERT INTO Roles_Privileges (role_id, privilege_id) VALUES (1, 1);
INSERT INTO Roles_Privileges (role_id, privilege_id) VALUES (1, 2);
INSERT INTO Roles_Privileges (role_id, privilege_id) VALUES (1, 3);

INSERT INTO Users_Roles (user_id, role_id) VALUES (1, 1);