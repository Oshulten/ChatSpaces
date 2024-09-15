/* eslint-disable react/react-in-jsx-scope */
import { SignedOut, SignedIn } from '@clerk/clerk-react';
import { createRootRoute, Navigate, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools';

export const Route = createRootRoute({
    component: () => (
        <div className='h-screen flex flex-col items-center justify-center'>
            <SignedOut>
                <Navigate to="/login" />
            </SignedOut>
            <SignedIn>
                <Navigate to="/dashboard" />
            </SignedIn>
            <Outlet />
            <TanStackRouterDevtools />
        </div>
    ),
})