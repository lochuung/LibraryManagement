CREATE TABLE Users (
       id bigint NOT NULL IDENTITY,
       created_date datetime2(6) DEFAULT NULL,
       updated_date datetime2(6) DEFAULT NULL,
       birthday nvarchar(255) DEFAULT NULL,
       email nvarchar(255) DEFAULT NULL,
       name nvarchar(255) DEFAULT NULL,
       password nvarchar(255) DEFAULT NULL,
       username nvarchar(255) DEFAULT NULL,
       PRIMARY KEY (id),
       CONSTRAINT UK_user_email UNIQUE (email),
       CONSTRAINT UK_user_username UNIQUE (username)
) ;

CREATE TABLE Roles (
       id bigint NOT NULL IDENTITY,
       name nvarchar(255) DEFAULT NULL,
       PRIMARY KEY (id)
) ;

CREATE TABLE Privileges (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id)
) ;

CREATE TABLE Roles_Privileges (
      role_id bigint NOT NULL,
      privilege_id bigint NOT NULL
,
      CONSTRAINT FK_roles_privileges_privilege_id FOREIGN KEY (privilege_id) REFERENCES Privileges (id),
      CONSTRAINT FK_roles_privileges_role_id FOREIGN KEY (role_id) REFERENCES Roles (id),
        PRIMARY KEY (role_id, privilege_id)
) ;

CREATE INDEX FK_roles_privileges_privilege_id ON Roles_Privileges (privilege_id);
CREATE INDEX FK_roles_privileges_role_id ON Roles_Privileges (role_id);

CREATE TABLE Users_Roles (
     user_id bigint NOT NULL,
     role_id bigint NOT NULL
,
     CONSTRAINT FK_users_roles_user_id FOREIGN KEY (user_id) REFERENCES Users (id),
     CONSTRAINT FK_users_roles_role_id FOREIGN KEY (role_id) REFERENCES Roles (id),
         PRIMARY KEY (user_id, role_id)
) ;

CREATE INDEX FK_users_roles_role_id ON Users_Roles (role_id);
CREATE INDEX FK_users_roles_user_id ON Users_Roles (user_id);

CREATE TABLE Category (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id)
) ;

CREATE TABLE Author (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE Publisher (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id)
) ;

CREATE TABLE Book (
    id bigint NOT NULL IDENTITY,
    title nvarchar(255) DEFAULT NULL,
    price decimal(19,2) DEFAULT NULL,
    image nvarchar(255) DEFAULT NULL,
    manufacture_date datetime2(6) DEFAULT NULL,
    added_date datetime2(6) DEFAULT NULL,
    status nvarchar(255) DEFAULT NULL,
    publisher_id bigint DEFAULT NULL,
    PRIMARY KEY (id),
    CONSTRAINT FK_Book_publisher_id FOREIGN KEY (publisher_id) REFERENCES Publisher (id)
) ;

CREATE TABLE Book_Author (
    book_id bigint NOT NULL,
    author_id bigint NOT NULL,
    CONSTRAINT FK_Book_Author_author_id FOREIGN KEY (author_id) REFERENCES Author (id),
    CONSTRAINT FK_Book_Author_book_id FOREIGN KEY (book_id) REFERENCES Book (id),
    PRIMARY KEY (book_id, author_id)
) ;

CREATE INDEX FK_Book_Author_author_id ON Book_Author (author_id);
CREATE INDEX FK_Book_Author_book_id ON Book_Author (book_id);

CREATE TABLE Book_Category (
    book_id bigint NOT NULL,
    category_id bigint NOT NULL,
    CONSTRAINT FK_Book_Category_book_id FOREIGN KEY (book_id) REFERENCES Book (id),
    CONSTRAINT FK_Book_Category_category_id FOREIGN KEY (category_id) REFERENCES Category (id),
    PRIMARY KEY (book_id, category_id)
) ;

CREATE INDEX FK_Book_Category_book_id ON Book_Category (book_id);
CREATE INDEX FK_Book_Category_category_id ON Book_Category (category_id);

CREATE TABLE ReaderType (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE Reader (
    id bigint NOT NULL IDENTITY,
    name nvarchar(255) DEFAULT NULL,
    dob datetime2(6) DEFAULT NULL,
    email nvarchar(255) DEFAULT NULL,
    debt decimal(19,2) DEFAULT 0.00,
    reader_type_id bigint DEFAULT NULL,
    created_date datetime2(6) DEFAULT NULL,
    updated_date datetime2(6) DEFAULT NULL,
    PRIMARY KEY (id),
    CONSTRAINT FK_Reader_reader_type_id FOREIGN KEY (reader_type_id) REFERENCES ReaderType (id)
) ;

CREATE TABLE Payment (
    id bigint NOT NULL IDENTITY,
    amount decimal(19,2) DEFAULT 0.00,
    payment_date datetime2(6) DEFAULT NULL,
    reader_id bigint DEFAULT NULL,
    PRIMARY KEY (id),
    CONSTRAINT FK_Payment_reader_id FOREIGN KEY (reader_id) REFERENCES Reader (id)
);

CREATE TABLE BookReader (
    id bigint NOT NULL IDENTITY,
    book_id bigint DEFAULT NULL,
    reader_id bigint DEFAULT NULL,
    borrow_date datetime2(6) DEFAULT NULL,
    return_date datetime2(6) DEFAULT NULL,
    status nvarchar(255) DEFAULT NULL,
    PRIMARY KEY (id),
    CONSTRAINT FK_BookReader_book_id FOREIGN KEY (book_id) REFERENCES Book (id),
    CONSTRAINT FK_BookReader_reader_id FOREIGN KEY (reader_id) REFERENCES Reader (id),
) ;