
import { AppComponent } from './app.component';
import { NgModule } from '@angular/core';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import {
    MatNativeDateModule, MatSelectModule, MatSlideToggleModule,
    MatTabsModule, MatProgressSpinnerModule, MatRadioModule,
    MatChipsModule, MatCardModule, MatDatepickerModule, MatInputModule,
    MatAutocompleteModule, MatToolbarModule, MatIconModule, MatMenuModule,
    MatCheckboxModule, MatButtonModule, MatStepperModule, MatDivider, MatDividerModule, MatCardActions
} from '@angular/material';
@NgModule({
    declarations: [],
    imports: [
        MatButtonModule,
        MatCheckboxModule,
        MatMenuModule,
        MatIconModule,
        MatToolbarModule,
        MatAutocompleteModule,
        MatInputModule,
        MatNativeDateModule,
        MatDatepickerModule,
        MatSelectModule,
        MatCardModule,
        MatChipsModule,
        MatSlideToggleModule,
        MatTabsModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatStepperModule,
        MatDividerModule,
        MatButtonToggleModule
    ],
    exports: [
        MatButtonModule,
        MatCheckboxModule,
        MatMenuModule,
        MatIconModule,
        MatToolbarModule,
        MatAutocompleteModule,
        MatInputModule,
        MatNativeDateModule,
        MatDatepickerModule,
        MatSelectModule,
        MatCardModule,
        MatChipsModule,
        MatSlideToggleModule,
        MatTabsModule,
        MatProgressSpinnerModule,
        MatRadioModule,
        MatStepperModule,
        MatDividerModule,
        MatButtonToggleModule
    ]
})

export class MaterialModule { }

