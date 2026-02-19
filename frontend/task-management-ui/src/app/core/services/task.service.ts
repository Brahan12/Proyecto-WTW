import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getTasks(status?: string, priority?: string) {
    let url = `${environment.apiUrl}/tasks?`;

    if (status) url += `status=${status}&`;
    if (priority) url += `priority=${priority}&`;

    return this.http.get<any[]>(url);
  }

  createTask(data: any) {
    return this.http.post(`${this.apiUrl}/tasks`, data);
  }

  updateStatus(id: number, status: string) {
    return this.http.put(`${this.apiUrl}/tasks/${id}/status`, { status });
  }
}
