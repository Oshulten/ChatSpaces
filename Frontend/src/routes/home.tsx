/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute('/home')({
  component: () => <>
    <h1>Home</h1>
  </>
})
