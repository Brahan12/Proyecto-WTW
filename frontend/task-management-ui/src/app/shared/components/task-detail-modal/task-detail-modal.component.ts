import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-detail-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-detail-modal.component.html'
})
export class TaskDetailModalComponent {

  @Input() show: boolean = false;
  @Input() title: string = "Detalle de tarea";

  @Input() task: any = null;
  @Input() userLabel: string = '';

  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }

  getExtraDataObject(): any {
    if (!this.task?.extraData) return null;

    try {
      return JSON.parse(this.task.extraData);
    } catch {
      return null;
    }
  }

  getEstimatedFinishDate(): string | null {
    const obj = this.getExtraDataObject();
    return obj?.estimatedFinishDate || null;
  }

  getMetadataEntries(): { key: string; value: any }[] | null {
    const obj = this.getExtraDataObject();

    if (!obj?.metadata) return null;

    const metadata = obj.metadata;

    if (typeof metadata === 'string') {
      return [{ key: 'Detalle', value: metadata }];
    }

    if (typeof metadata === 'object' && !Array.isArray(metadata)) {
      return Object.entries(metadata).map(([key, value]) => ({
        key,
        value
      }));
    }

    return [{ key: 'Información', value: String(metadata) }];
  }

  getTags(): string[] {
    const obj = this.getExtraDataObject();
    return obj?.tags || [];
  }

  formatJson(json: string | null | undefined): string {
    if (!json) return "No aplica";

    try {
      const obj = JSON.parse(json);
      return JSON.stringify(obj, null, 2);
    } catch {
      return json;
    }
  }
}
