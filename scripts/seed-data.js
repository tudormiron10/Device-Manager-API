// Seed data script for DeviceManagerDB

db = db.getSiblingDB('DeviceManagerDB');

print('--- Seeding DeviceManagerDB ---');

// --- Seed Users ---
const users = [
    {
        name: 'John Smith',
        email: 'john.smith@company.com',
        passwordHash: '',
        role: 'Developer',
        location: 'London, UK',
        createdAt: new Date()
    },
    {
        name: 'Ana Popescu',
        email: 'ana.popescu@company.com',
        passwordHash: '',
        role: 'QA Engineer',
        location: 'Bucharest, Romania',
        createdAt: new Date()
    },
    {
        name: 'Michael Chen',
        email: 'michael.chen@company.com',
        passwordHash: '',
        role: 'Project Manager',
        location: 'New York, USA',
        createdAt: new Date()
    },
    {
        name: 'Sophie Bernard',
        email: 'sophie.bernard@company.com',
        passwordHash: '',
        role: 'Designer',
        location: 'Paris, France',
        createdAt: new Date()
    },
    {
        name: 'Alex Muller',
        email: 'alex.muller@company.com',
        passwordHash: '',
        role: 'DevOps Engineer',
        location: 'Berlin, Germany',
        createdAt: new Date()
    }
];

users.forEach(user => {
    const result = db.users.updateOne(
        { email: user.email },
        { $setOnInsert: user },
        { upsert: true }
    );
    const action = result.upsertedCount > 0 ? 'Inserted' : 'Already exists';
    print(`${action}: ${user.name} (${user.email})`);
});

const johnId = db.users.findOne({ email: 'john.smith@company.com' })._id;
const anaId = db.users.findOne({ email: 'ana.popescu@company.com' })._id;
const michaelId = db.users.findOne({ email: 'michael.chen@company.com' })._id;

// --- Seed Devices ---
const devices = [
    {
        name: 'iPhone 16 Pro',
        manufacturer: 'Apple',
        type: 'phone',
        os: 'iOS',
        osVersion: '18.2',
        processor: 'A18 Pro',
        ramAmount: '8GB',
        location: 'London Office - Floor 3',
        description: 'Flagship Apple smartphone with advanced camera system and titanium design.',
        assignedUserId: johnId,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Galaxy S25 Ultra',
        manufacturer: 'Samsung',
        type: 'phone',
        os: 'Android',
        osVersion: '15',
        processor: 'Snapdragon 8 Elite',
        ramAmount: '12GB',
        location: 'Bucharest Office - QA Lab',
        description: 'Premium Samsung smartphone with S Pen and AI-powered features.',
        assignedUserId: anaId,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Pixel 9 Pro',
        manufacturer: 'Google',
        type: 'phone',
        os: 'Android',
        osVersion: '15',
        processor: 'Tensor G4',
        ramAmount: '16GB',
        location: 'Storage Room A',
        description: 'Google flagship with best-in-class AI and photography capabilities.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'iPad Pro 13-inch M4',
        manufacturer: 'Apple',
        type: 'tablet',
        os: 'iPadOS',
        osVersion: '18.2',
        processor: 'Apple M4',
        ramAmount: '16GB',
        location: 'New York Office - Meeting Room B',
        description: 'Professional-grade tablet with OLED display and desktop-class performance.',
        assignedUserId: michaelId,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Galaxy Tab S10 Ultra',
        manufacturer: 'Samsung',
        type: 'tablet',
        os: 'Android',
        osVersion: '15',
        processor: 'MediaTek Dimensity 9300+',
        ramAmount: '16GB',
        location: 'Storage Room A',
        description: 'Large-screen Samsung tablet designed for productivity and creativity.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'OnePlus 13',
        manufacturer: 'OnePlus',
        type: 'phone',
        os: 'Android',
        osVersion: '15',
        processor: 'Snapdragon 8 Elite',
        ramAmount: '16GB',
        location: 'Berlin Office - Dev Area',
        description: 'High-performance smartphone with fast charging and fluid display.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'iPhone SE 4',
        manufacturer: 'Apple',
        type: 'phone',
        os: 'iOS',
        osVersion: '18.2',
        processor: 'A18',
        ramAmount: '8GB',
        location: 'London Office - Reception',
        description: 'Affordable Apple smartphone with modern design and capable performance.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Surface Pro 10',
        manufacturer: 'Microsoft',
        type: 'tablet',
        os: 'Windows',
        osVersion: '11',
        processor: 'Intel Core Ultra 7',
        ramAmount: '32GB',
        location: 'Paris Office - Design Studio',
        description: 'Versatile 2-in-1 tablet that replaces your laptop with full Windows experience.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Xiaomi 15 Pro',
        manufacturer: 'Xiaomi',
        type: 'phone',
        os: 'Android',
        osVersion: '15',
        processor: 'Snapdragon 8 Elite',
        ramAmount: '16GB',
        location: 'Storage Room B',
        description: 'Feature-rich smartphone with Leica optics and premium build quality.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'iPad Air M3',
        manufacturer: 'Apple',
        type: 'tablet',
        os: 'iPadOS',
        osVersion: '18.2',
        processor: 'Apple M3',
        ramAmount: '8GB',
        location: 'New York Office - Lounge',
        description: 'Lightweight yet powerful tablet for everyday productivity and entertainment.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Galaxy Z Fold 6',
        manufacturer: 'Samsung',
        type: 'phone',
        os: 'Android',
        osVersion: '15',
        processor: 'Snapdragon 8 Gen 3',
        ramAmount: '12GB',
        location: 'London Office - Floor 3',
        description: 'Foldable smartphone that unfolds into a tablet-sized display.',
        assignedUserId: johnId,
        createdAt: new Date(),
        updatedAt: new Date()
    },
    {
        name: 'Pixel Tablet 2',
        manufacturer: 'Google',
        type: 'tablet',
        os: 'Android',
        osVersion: '15',
        processor: 'Tensor G4',
        ramAmount: '12GB',
        location: 'Bucharest Office - Common Area',
        description: 'Versatile Android tablet with smart home hub functionality.',
        assignedUserId: null,
        createdAt: new Date(),
        updatedAt: new Date()
    }
];

devices.forEach(device => {
    const result = db.devices.updateOne(
        { name: device.name, manufacturer: device.manufacturer },
        { $setOnInsert: device },
        { upsert: true }
    );
    const action = result.upsertedCount > 0 ? 'Inserted' : 'Already exists';
    print(`${action}: ${device.manufacturer} ${device.name}`);
});

print('');
print(`Total users: ${db.users.countDocuments()}`);
print(`Total devices: ${db.devices.countDocuments()}`);
print('--- Seeding complete ---');
