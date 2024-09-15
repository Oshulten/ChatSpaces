/* eslint-disable react/react-in-jsx-scope */
import { createFileRoute } from '@tanstack/react-router'
import useClientUser from '../hooks/useClientUser';
import { UserButton } from '@clerk/clerk-react';

export const Route = createFileRoute('/dashboard')({
  component: Dashboard
})

export default function Dashboard() {
  const { clientUser } = useClientUser();

  if (!clientUser) return null;

  return (
    <>
      <h1>Dashboard</h1>
      <UserButton />
      <p>{clientUser.username}</p>
    </>
  );
}