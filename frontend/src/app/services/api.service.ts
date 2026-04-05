import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DeviceDto, CreateDeviceDto, UpdateDeviceDto, UserDto } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);
  private readonly BASE_URL = 'http://localhost:5000/api';

  getDevices(): Observable<DeviceDto[]> {
    return this.http.get<DeviceDto[]>(`${this.BASE_URL}/Devices`);
  }

  getDeviceById(id: string): Observable<DeviceDto> {
    return this.http.get<DeviceDto>(`${this.BASE_URL}/Devices/${id}`);
  }

  createDevice(device: CreateDeviceDto): Observable<DeviceDto> {
    return this.http.post<DeviceDto>(`${this.BASE_URL}/Devices`, device);
  }

  updateDevice(id: string, device: UpdateDeviceDto): Observable<void> {
    return this.http.put<void>(`${this.BASE_URL}/Devices/${id}`, device);
  }

  deleteDevice(id: string): Observable<void> {
    return this.http.delete<void>(`${this.BASE_URL}/Devices/${id}`);
  }

  getUsers(): Observable<UserDto[]> {
    return this.http.get<UserDto[]>(`${this.BASE_URL}/Users`);
  }

  assignDevice(id: string): Observable<DeviceDto> {
    return this.http.post<DeviceDto>(`${this.BASE_URL}/Devices/${id}/assign`, {});
  }

  unassignDevice(id: string): Observable<DeviceDto> {
    return this.http.post<DeviceDto>(`${this.BASE_URL}/Devices/${id}/unassign`, {});
  }
}
