import * as authRoles from '../../src/utils/authRoles.js'

afterEach(() => {
    const roles = sessionStorage.getItem('roles');

    if (roles != null) {
        sessionStorage.removeItem('roles');
    }
});

describe('getAuthRoles', () => {
    test('should get roles when roles session token exists', () => {
        sessionStorage.setItem('roles', 'Admin');
        
        expect(authRoles.getAuthRoles()).toBe('Admin');
    });

    test('should not get roles when roles session token does not exist', () => {
        expect(authRoles.getAuthRoles()).toBe(null);
    });    
})

describe('setAuthRoles', () => {
    test('should set roles when set auth roles function gets called', () => {
        authRoles.setAuthRoles('Admin')
        
        expect(sessionStorage.getItem('roles')).toBe('Admin');
    });
})

describe('clearAuthRoles', () => {
    test('should remove roles when roles session token exists', () => {
        sessionStorage.setItem('roles', 'Admin');
        authRoles.clearAuthRoles()
        
        expect(sessionStorage.getItem('roles')).toBe(null);
    });

    test('should do nothing when roles session token does not exist', () => {
        authRoles.clearAuthRoles()

        expect(sessionStorage.getItem('roles')).toBe(null);
    });
})