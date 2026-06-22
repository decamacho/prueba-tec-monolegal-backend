db = db.getSiblingDB('TechnicalTestMonolegalDB');

db.createCollection('invoicesClient');

db.invoices.insertMany([
    {
        codigoFactura: "FAC-2026-001",
        fechaEmision: new Date("2026-06-01T08:00:00Z"),
        cliente: {
            nombre: "Firma Abogados Asociados",
            documento: "1010101010",
            emailContacto: "test1@monolegal.co"
        },
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
            emailContacto: "test2@monolegal.co"
        },
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
            emailContacto: "test3@monolegal.co"
        },
        resumenFinanciero: {
            total: 85000.00
        },
        estado: "primerrecordatorio"
    }
]);

print("Base de datos TechnicalTestMonolegalDB inicializada con 3 facturas de prueba");