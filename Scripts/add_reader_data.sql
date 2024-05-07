-- add reader type data
INSERT INTO ReaderType (name) VALUES ('Student');
INSERT INTO ReaderType (name) VALUES ('Teacher');
INSERT INTO ReaderType (name) VALUES ('Professor');
INSERT INTO ReaderType (name) VALUES ('Other');

-- add reader data
INSERT INTO Reader (name, dob, email, debt, reader_type_id) VALUES ('John Doe', '1990-01-01',
                                                                    'john@gmail.com', 0, 1);
