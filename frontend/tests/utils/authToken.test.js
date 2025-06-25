import * as authToken from '../../src/utils/authToken.js'

afterEach(() => {
    const authToken = sessionStorage.getItem('token');

    if (authToken != null) {
        sessionStorage.removeItem('token');
    }
});

describe('getAuthToken', () => {
    test('should get auth token when auth token session token exists', () => {
        sessionStorage.setItem('token', 'token-12345-67890');

        expect(authToken.getAuthToken()).toBe('token-12345-67890');
    });

    test('should not get auth token when auth token session token does not exist', () => {
        expect(authToken.getAuthToken()).toBe(null);
    });
})

describe('setAuthToken', () => {
    test('should set auth token when set auth auth token function gets called', () => {
        authToken.setAuthToken('token-12345-67890')

        expect(sessionStorage.getItem('token')).toBe('token-12345-67890');
    });
})

describe('clearAuthToken', () => {
    test('should remove auth token when auth token session token exists', () => {
        sessionStorage.setItem('token', 'token-12345-67890');
        authToken.clearAuthToken()

        expect(sessionStorage.getItem('token')).toBe(null);
    });

    test('should do nothing when auth token session token does not exist', () => {
        authToken.clearAuthToken()

        expect(sessionStorage.getItem('token')).toBe(null);
    });
})