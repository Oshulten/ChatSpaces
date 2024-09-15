/* eslint-disable react/react-in-jsx-scope */
import { SignedOut, SignedIn, SignInButton } from '@clerk/clerk-react'
import { createFileRoute, Navigate } from '@tanstack/react-router'

export const Route = createFileRoute('/login')({
  component: () => <>
    <h1>Login</h1>
    <SignedOut>
      <SignInButton mode='modal'>
        <button className='btn'>Sign In</button>
      </SignInButton>
    </SignedOut>
    <SignedIn>
      <Navigate to="/dashboard" />
    </SignedIn>
  </>
})
