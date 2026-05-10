import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';
import { AddCategory } from './Features/Category/add-category/add-category';

export const routes: Routes = [

    {
        path : 'admin/categories',
        component : CategoryList
    },
    {
        path : 'admin/categories/add',
        component : AddCategory
    }
];
