import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatAnchor } from "@angular/material/button";

@Component({
  selector: 'app-confirmation-dialog',
  imports: [MatAnchor],
  templateUrl: './confirmation-dialog.html',
  styleUrl: './confirmation-dialog.css',
})
export class ConfirmationDialog {

  dialogRef = inject(MatDialogRef<ConfirmationDialog>);
  data = inject(MAT_DIALOG_DATA);

  onConfirm(){
    this.dialogRef.close(true);
  }
  onCancel(){
    this.dialogRef.close();
  }

}
