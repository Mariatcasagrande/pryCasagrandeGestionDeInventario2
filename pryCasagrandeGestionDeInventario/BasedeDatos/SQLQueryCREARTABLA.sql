CREATE TABLE productos (
    codigo VARCHAR(50) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    categoria VARCHAR(50),
    precio DECIMAL(10, 2) NOT NULL,
    stock INT NOT NULL,
    descripcion TEXT
);