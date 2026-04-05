export interface DeviceDto {
  id: string;
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description?: string;
  location?: string;
  assignedUserId?: string;
  assignedUserName?: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateDeviceDto {
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description?: string;
  location?: string;
}

export interface UpdateDeviceDto {
  name: string;
  manufacturer: string;
  type: string;
  os: string;
  osVersion: string;
  processor: string;
  ramAmount: string;
  description?: string;
  location?: string;
}

export interface UserDto {
  id: string;
  name: string;
  email: string;
  role: string;
  location: string;
  createdAt: Date;
}
