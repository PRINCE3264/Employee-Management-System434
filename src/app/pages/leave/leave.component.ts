
import { Component, OnInit, inject } from '@angular/core';
import { LeaveService } from '../../services/leave.service';
import { ILeave, LeaveStatus, LeaveType } from '../../types/leave';
import { PagedData } from '../../types/paged-data';
import { TableComponent } from '../../componets/table/table.component';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-leave',
  standalone: true,
  templateUrl: './leave.component.html',
  styleUrls: ['./leave.component.css'],
  imports: [TableComponent, MatPaginatorModule, CommonModule]
})
export class LeaveComponent implements OnInit {
  private leaveService = inject(LeaveService);
  private authService = inject(AuthService);

  filter = {
    pageIndex: 0,
    pageSize: 5
  };

  data: PagedData<ILeave> = { data: [], totalCount: 0 };

  showCols: any[] = [];

  ngOnInit(): void {
    this.setupColumns();
    this.getLeavesData();
  }

  setupColumns(): void {
    this.showCols = [
      'id',
      {
        key: 'type',
        format: (row: ILeave) => {
          switch (row.type) {
            case LeaveType.Casual: return 'Casual Leave';
            case LeaveType.Sick: return 'Sick Leave';
            case LeaveType.Earned: return 'Earned Leave';
            default: return row.type;
          }
        }
      },
      'reason',
      'leaveDate',
      {
        key: 'status',
        format: (row: ILeave) => {
          switch (row.status) {
            case LeaveStatus.Pending: return 'Pending';
            case LeaveStatus.Rejected: return 'Rejected';
            case LeaveStatus.Accepted: return 'Accepted';
            case LeaveStatus.Cancelled: return 'Cancelled';
            default: return row.status;
          }
        }
      },
      {
        key: 'action',
        buttons: (row: ILeave) => {
          if (this.authService.isAdmin) {
            if (row.status === LeaveStatus.Pending) {
              return ['Accept', 'Reject'];
            }
          } else {
            if (row.status === LeaveStatus.Pending) {
              return ['Cancel'];
            }
          }
          return [];
        }
      }
    ];
  }

  getLeavesData(): void {
    this.leaveService.getLeaves(this.filter).subscribe(result => {
      this.data = result;
    });
  }

  onPageChange(event: PageEvent): void {
    this.filter.pageIndex = event.pageIndex;
    this.filter.pageSize = event.pageSize;
    this.getLeavesData();
  }

  handleRowAction(event: { btn: string, rowData: ILeave }) {
    const { btn, rowData } = event;

    switch (btn) {
      case 'Cancel':
        this.cancelLeave(rowData);
        break;
      case 'Accept':
        this.acceptLeave(rowData);
        break;
      case 'Reject':
        this.rejectLeave(rowData);
        break;
      default:
        console.warn('Unhandled action:', btn);
    }
  }

  cancelLeave(leave: ILeave): void {
    if (confirm('Are you sure you want to cancel this leave?')) {
      this.leaveService.cancelLeave(leave.id).subscribe(() => {
        alert('Leave cancelled.');
        this.getLeavesData();
      });
    }
  }

  acceptLeave(leave: ILeave): void {
    if (confirm('Are you sure you want to accept this leave?')) {
      this.leaveService.acceptLeave(leave.id).subscribe(() => {
        alert('Leave accepted.');
        this.getLeavesData();
      });
    }
  }

  rejectLeave(leave: ILeave): void {
    if (confirm('Are you sure you want to reject this leave?')) {
      this.leaveService.rejectLeave(leave.id).subscribe(() => {
        alert('Leave rejected.');
        this.getLeavesData();
      });
    }
  }
}
