import { useQuery } from "@tanstack/react-query";
import { ClientUser } from "../types/types";
import { Clerk } from "@clerk/clerk-js";

export default function useClientUser() {
    const queryKey = ["clientUser"];

    const clientUserQuery = useQuery({
        queryKey,
        queryFn: async (): Promise<ClientUser | undefined> => {
            const clerk = new Clerk(import.meta.env.VITE_CLERK_PUBLISHABLE_KEY)

            await clerk.load({})

            const clerkUser = clerk.user;

            if (!clerkUser) {
                console.error("Clerk is not available")
                return;
            }

            const clientUser: ClientUser = {
                username: clerkUser.username!,
                id: clerkUser.id,
                imageUrl: clerkUser.imageUrl
            }

            return clientUser;
        },
    });

    return {
        clientUser: clientUserQuery.data,
    }
}