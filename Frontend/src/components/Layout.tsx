import React from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';

export let Layout = ({children}:{children:React.ReactNode}) => {
    return (
        <div>
            <NavMenu />
            <Container>
                {children}
            </Container>
        </div>
    );
}
