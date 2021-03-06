import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})

export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(component: MemberEditComponent): boolean {
    if(component.changes || (component.editForm.dirty && !component.changes)) {
      return confirm('Are you sure you want to leave. Any unsaved changes will be lost.')
    }
    return true;
  }
  
}
