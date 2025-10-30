


import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [MatTableModule, MatCardModule, MatButtonModule],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent {
  @Input() data: any[] = [];

  @Input() displayedColumns: (
    string | {
      key: string;
      format?: (row: any) => string;
      buttons?: string[] | ((row: any) => string[]);
    }
  )[] = [];

  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();
  @Output() rowClick = new EventEmitter<{ btn: string, rowData: any }>();
  @Output() onAttendance = new EventEmitter<any>(); // ideally EventEmitter<IEmployee>


  // Emit default edit
  edit(row: any) {
    this.onEdit.emit(row);
  }

  // Emit default delete
  delete(row: any) {
    this.onDelete.emit(row);
  }

  // Get column key for headers and cells
  getColumnKey(col: any): string {
    return typeof col === 'string' ? col : col.key;
  }

  // Format value for display
  getFormattedValue(row: any, col: any): string {
    if (typeof col === 'string') {
      return this.getNestedValue(row, col);
    } else if (col.format) {
      return col.format(row);
    } else {
      return this.getNestedValue(row, col.key);
    }
  }

  // Access nested property like "user.name"
  getNestedValue(obj: any, path: string): any {
    return path.split('.').reduce((acc, key) => acc?.[key], obj);
  }

  // Return readable column title
  getHeaderTitle(col: any): string {
    const key = this.getColumnKey(col);
    const headerMap: { [key: string]: string } = {
      id: 'ID',
      name: 'Name',
      email: 'Email',
      phone: 'Phone',
      type: 'Type',
      reason: 'Reason',
      leaveDate: 'Leave Date',
      status: 'Status',
      action: 'Actions'
    };
    return headerMap[key] || this.capitalizeWords(key);
  }

  // Capitalize snake_case or camelCase into readable form
  capitalizeWords(text: string): string {
    return text
      .replace(/([A-Z])/g, ' $1')
      .replace(/[_\-]/g, ' ')
      .replace(/\b\w/g, c => c.toUpperCase());
  }

  // Emit custom button action
  onButtonClick(btn: string, rowData: any) {
    this.rowClick.emit({ btn, rowData });
  }

  // Type guard
  isColumnObject(col: any): col is {
    key: string;
    format?: (row: any) => string;
    buttons?: string[] | ((row: any) => string[]);
  } {
    return typeof col === 'object' && col !== null && 'key' in col;
  }

  // Extract dynamic or static buttons
  getButtons(col: any, row: any): string[] {
    if (this.isColumnObject(col)) {
      if (typeof col.buttons === 'function') {
        return col.buttons(row);
      } else if (Array.isArray(col.buttons)) {
        return col.buttons;
      }
    }
    return [];
  }

  // Button color map
  getButtonColor(btn: string): 'primary' | 'accent' | 'warn' {
    const map: { [key: string]: 'primary' | 'accent' | 'warn' } = {
      edit: 'primary',
      delete: 'warn',
      cancel: 'warn',
      reject: 'warn',
      accept: 'accent',
    };
    return map[btn.toLowerCase()] || 'primary';
  }
}
