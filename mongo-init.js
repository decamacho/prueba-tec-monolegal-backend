// Seleccionar la base de datos
db = db.getSiblingDB('TechnicalTestMonolegalDB');

db.invoices.drop();

db.createCollection('invoices');

db.invoices.insertMany([
    {
        codigoFactura: "FAC-2026-001",
        fechaEmision: new Date("2026-06-01T08:00:00Z"),
        cliente: {
            nombre: "Firma Abogados Asociados",
            documento: "1010101010",
            emailContacto: "decamacho66@gmail.com",
            telefono: "3001112233",
            direccion: "Edificio Centro Empresarial, Of 401"
        },
        items: [
            { descripcion: "Licencia Plataforma Vigilancia Judicial", cantidad: 1, precioUnitario: 150000.00, subtotal: 150000.00 }
        ],
        resumenFinanciero: { total: 150000.00 },
        estado: "primerrecordatorio"
    },
    {
        codigoFactura: "FAC-2026-004",
        fechaEmision: new Date("2026-06-08T09:30:00Z"),
        cliente: {
            nombre: "Firma Abogados Asociados",
            documento: "1010101010",
            emailContacto: "decamacho66@gmail.com",
            telefono: "3001112233",
            direccion: "Edificio Centro Empresarial, Of 401"
        },
        items: [
            { descripcion: "Horas de consultoría extra", cantidad: 3, precioUnitario: 100000.00, subtotal: 300000.00 }
        ],
        resumenFinanciero: { total: 300000.00 },
        estado: "primerrecordatorio"
    },
    {
        codigoFactura: "FAC-2026-009",
        fechaEmision: new Date("2026-06-18T14:15:00Z"),
        cliente: {
            nombre: "Firma Abogados Asociados",
            documento: "1010101010",
            emailContacto: "decamacho66@gmail.com",
            telefono: "3001112233",
            direccion: "Edificio Centro Empresarial, Of 401"
        },
        items: [
            { descripcion: "Actualización de software legal", cantidad: 1, precioUnitario: 210000.00, subtotal: 210000.00 }
        ],
        resumenFinanciero: { total: 210000.00 },
        estado: "segundorecordatorio"
    },

    {
        codigoFactura: "FAC-2026-002",
        fechaEmision: new Date("2026-06-05T08:00:00Z"),
        cliente: {
            nombre: "Consultoría Jurídica SAS",
            documento: "2020202020",
            emailContacto: "tabletasamsumg2015@gmail.com",
            telefono: "3109998877",
            direccion: "Calle 100 # 15-20"
        },
        items: [
            { descripcion: "Asesoría Legal Corporativa", cantidad: 2, precioUnitario: 160000.00, subtotal: 320000.00 }
        ],
        resumenFinanciero: { total: 320000.00 },
        estado: "segundorecordatorio"
    },
    {
        codigoFactura: "FAC-2026-005",
        fechaEmision: new Date("2026-06-10T11:00:00Z"),
        cliente: {
            nombre: "Consultoría Jurídica SAS",
            documento: "2020202020",
            emailContacto: "tabletasamsumg2015@gmail.com",
            telefono: "3109998877",
            direccion: "Calle 100 # 15-20"
        },
        items: [
            { descripcion: "Redacción de contratos comerciales", cantidad: 1, precioUnitario: 125000.00, subtotal: 125000.00 }
        ],
        resumenFinanciero: { total: 125000.00 },
        estado: "primerrecordatorio"
    },

    {
        codigoFactura: "FAC-2026-003",
        fechaEmision: new Date("2026-06-06T10:00:00Z"),
        cliente: {
            nombre: "Defensa Legal Corp",
            documento: "3030303030",
            emailContacto: "emailsendtechnicaltestreport@gmail.com",
            telefono: "3204445566",
            direccion: "Carrera 7 # 72-10"
        },
        items: [
            { descripcion: "Revisión de Contratos Laborales", cantidad: 1, precioUnitario: 85000.00, subtotal: 85000.00 }
        ],
        resumenFinanciero: { total: 85000.00 },
        estado: "primerrecordatorio"
    },
    {
        codigoFactura: "FAC-2026-007",
        fechaEmision: new Date("2026-06-14T08:30:00Z"),
        cliente: {
            nombre: "Defensa Legal Corp",
            documento: "3030303030",
            emailContacto: "emailsendtechnicaltestreport@gmail.com",
            telefono: "3204445566",
            direccion: "Carrera 7 # 72-10"
        },
        items: [
            { descripcion: "Representación en audiencias", cantidad: 2, precioUnitario: 170000.00, subtotal: 340000.00 }
        ],
        resumenFinanciero: { total: 340000.00 },
        estado: "primerrecordatorio"
    },
    {
        codigoFactura: "FAC-2026-010",
        fechaEmision: new Date("2026-06-20T16:00:00Z"),
        cliente: {
            nombre: "Defensa Legal Corp",
            documento: "3030303030",
            emailContacto: "emailsendtechnicaltestreport@gmail.com",
            telefono: "3204445566",
            direccion: "Carrera 7 # 72-10"
        },
        items: [
            { descripcion: "Gestión documental", cantidad: 1, precioUnitario: 115000.00, subtotal: 115000.00 }
        ],
        resumenFinanciero: { total: 115000.00 },
        estado: "primerrecordatorio"
    },

    {
        codigoFactura: "FAC-2026-006",
        fechaEmision: new Date("2026-06-12T09:00:00Z"),
        cliente: {
            nombre: "Servicios Legales Andinos",
            documento: "4040404040",
            emailContacto: "legal@andinos.co",
            telefono: "3155556677",
            direccion: "Av. Chile # 7-45"
        },
        items: [
            { descripcion: "Auditoría de cumplimiento legal", cantidad: 1, precioUnitario: 600000.00, subtotal: 600000.00 }
        ],
        resumenFinanciero: { total: 600000.00 },
        estado: "segundorecordatorio"
    },
    {
        codigoFactura: "FAC-2026-011",
        fechaEmision: new Date("2026-06-22T10:45:00Z"),
        cliente: {
            nombre: "Servicios Legales Andinos",
            documento: "4040404040",
            emailContacto: "legal@andinos.co",
            telefono: "3155556677",
            direccion: "Av. Chile # 7-45"
        },
        items: [
            { descripcion: "Trámite de registros de marca", cantidad: 1, precioUnitario: 530000.00, subtotal: 530000.00 }
        ],
        resumenFinanciero: { total: 530000.00 },
        estado: "primerrecordatorio"
    },

    {
        codigoFactura: "FAC-2026-008",
        fechaEmision: new Date("2026-06-15T13:20:00Z"),
        cliente: {
            nombre: "Corporación Justicia Activa",
            documento: "5050505050",
            emailContacto: "contacto@justiciaactiva.co",
            telefono: "3182223344",
            direccion: "Calle 93 # 11-22"
        },
        items: [
            { descripcion: "Consultoría en derecho penal", cantidad: 4, precioUnitario: 200000.00, subtotal: 800000.00 }
        ],
        resumenFinanciero: { total: 800000.00 },
        estado: "primerrecordatorio"
    },

    {
        codigoFactura: "FAC-2026-012",
        fechaEmision: new Date("2026-06-23T08:15:00Z"),
        cliente: {
            nombre: "Asesores Derecho Integral",
            documento: "6060606060",
            emailContacto: "info@derechointegral.co",
            telefono: "3004449988",
            direccion: "Carrera 15 # 88-12"
        },
        items: [
            { descripcion: "Soporte legal mensual", cantidad: 1, precioUnitario: 275000.00, subtotal: 275000.00 }
        ],
        resumenFinanciero: { total: 275000.00 },
        estado: "segundorecordatorio"
    }
]);

print("Base de datos MonolegalDB inicializada con 12 facturas de prueba y estructura actualizada");