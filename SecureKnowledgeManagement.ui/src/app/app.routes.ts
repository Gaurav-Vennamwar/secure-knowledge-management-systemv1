import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';

export const routes: Routes = [

    {
        path : 'admin/categories',
        component : CategoryList
    }
];
