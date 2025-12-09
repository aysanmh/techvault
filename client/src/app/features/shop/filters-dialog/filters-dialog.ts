import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/ShopService';
import {MatDivider} from '@angular/material/divider'
import {MatListOption, MatSelectionList} from '@angular/material/list'
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-filters-dialog',
  imports: [
    CommonModule,
    MatDivider,
    MatSelectionList,
    MatListOption,
    MatButton,
    FormsModule
  ],
  templateUrl: './filters-dialog.html',
  styleUrl: './filters-dialog.scss',
})
export class FiltersDialog {

  shopService = inject(ShopService);

  private dialogRef = inject(MatDialogRef<FiltersDialog>);

  data = inject(MAT_DIALOG_DATA);

  selectedBrands: string[] = this.data.selectedBrands;
  selectedGroups: string[] = this.data.selectedGroups;

  applyFilters() {

    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedGroups: this.selectedGroups
    })

  }

}
