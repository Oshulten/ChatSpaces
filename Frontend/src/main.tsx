/* eslint-disable react/react-in-jsx-scope */
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { routeTree } from './routeTree.gen'
import { createRouter, RouterProvider } from '@tanstack/react-router';
import { DefaultCatchBoundary } from './components/DefaultCatchBoundary.tsx';
import { NotFound } from './components/NotFound.tsx';
import './css/index.css'
import { ClerkProvider } from '@clerk/clerk-react';

const queryClient = new QueryClient();


const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
  defaultPreload: 'intent',
  defaultPreloadStaleTime: 0,
  defaultErrorComponent: DefaultCatchBoundary,
  defaultNotFoundComponent: () => <NotFound />,
})

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

<<<<<<< HEAD
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <ClerkProvider publishableKey={import.meta.env.VITE_CLERK_PUBLISHABLE_KEY}>
        <RouterProvider router={router} />
      </ClerkProvider>
    </QueryClientProvider>
  </StrictMode>,
=======

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <ClerkProvider publishableKey={import.meta.env.VITE_CLERK_PUBLISHABLE_KEY} afterSignOutUrl="/">
        <RouterProvider router={router} />
      </ClerkProvider>
    </QueryClientProvider>
  </StrictMode >,
>>>>>>> cf35211b14c5459f186e6e54c6dc656c08339ea9
)
