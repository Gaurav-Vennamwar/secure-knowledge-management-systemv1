import { Routes } from '@angular/router';
import { CategoryList } from './Features/Category/category-list/category-list';
import { AddCategory } from './Features/Category/add-category/add-category';
import { EditCategory } from './Features/Category/edit-category/edit-category';

export const routes: Routes = [

    {
        path : 'admin/categories',
        component : CategoryList
    },
    {
        path : 'admin/categories/add',
        component : AddCategory
    },
    {
        path : 'admin/categories/edit/:id',
        component : EditCategory

    }
];
