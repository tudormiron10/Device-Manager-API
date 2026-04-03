// Database creation script for DeviceManagerDB

db = db.getSiblingDB('DeviceManagerDB');

print('--- Creating DeviceManagerDB ---');

const existingCollections = db.getCollectionNames();

if (!existingCollections.includes('devices')) {
    db.createCollection('devices');
    print('Created collection: devices');
} else {
    print('Collection already exists: devices');
}

if (!existingCollections.includes('users')) {
    db.createCollection('users');
    print('Created collection: users');
} else {
    print('Collection already exists: users');
}

db.users.createIndex({ email: 1 }, { unique: true });
print('Index created: users.email (unique)');

db.devices.createIndex({ name: 1, manufacturer: 1 });
print('Index created: devices.name + manufacturer (compound)');

db.devices.createIndex(
    {
        name: 'text',
        manufacturer: 'text',
        processor: 'text',
        ramAmount: 'text'
    },
    {
        weights: {
            name: 10,
            manufacturer: 7,
            processor: 5,
            ramAmount: 3
        },
        name: 'device_text_search'
    }
);
print('Text index created: device_text_search (weighted)');

print('--- Database setup complete ---');
