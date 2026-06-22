// Seleccionar la base de datos
db = db.getSiblingDB('TechnicalTestMonolegalDB');

// Limpiar la coleccion si ya existe (util para cuando se reinicie Docker)
db.invoices.drop();

// Crear la coleccion de facturas
db.createCollection('invoices');

// Insertar las 3 facturas de prueba requeridas con la estructura actualizada
db.invoices.insertMany([
    {
        codigoFactura: "FAC-2026-001",
        fechaEmision: new Date("2026-06-01T08:00:00Z"),
        cliente: {
            nombre: "Firma Abogados Asociados",
            documento: "1010101010",
            emailContacto: "test1@monolegal.co",
            telefono: "3001112233",
            direccion: "Edificio Centro Empresarial, Of 401"
        },
        items: [
            {
                descripcion: "Licencia Plataforma Vigilancia Judicial",
                cantidad: 1,
                precioUnitario: 150000.00,
                subtotal: 150000.00
            }
        ],
        resumenFinanciero: {
            total: 150000.00
        },
        estado: "primerrecordatorio"
    },
    {
        codigoFactura: "FAC-2026-002",
        fechaEmision: new Date("2026-06-10T08:00:00Z"),
        cliente: {
            nombre: "Consultoría Jurídica SAS",
            documento: "2020202020",
            emailContacto: "test2@monolegal.co",
            telefono: "3109998877",
            direccion: "Calle 100 # 15-20"
        },
        items: [
            {
                descripcion: "Asesoría Legal Corporativa",
                cantidad: 2,
                precioUnitario: 160000.00,
                subtotal: 320000.00
            }
        ],
        resumenFinanciero: {
            total: 320000.00
        },
        estado: "segundorecordatorio"
    },
    {
        codigoFactura: "FAC-2026-003",
        fechaEmision: new Date("2026-06-15T08:00:00Z"),
        cliente: {
            nombre: "Defensa Legal Corp",
            documento: "3030303030",
            emailContacto: "test3@monolegal.co",
            telefono: "3204445566",
            direccion: "Carrera 7 # 72-10"
        },
        items: [
            {
                descripcion: "Revisión de Contratos Laborales",
                cantidad: 1,
                precioUnitario: 85000.00,
                subtotal: 85000.00
            }
        ],
        resumenFinanciero: {
            total: 85000.00
        },
        estado: "primerrecordatorio"
    }
]);

print("Base de datos MonolegalDB inicializada con 3 facturas de prueba y estructura actualizada");