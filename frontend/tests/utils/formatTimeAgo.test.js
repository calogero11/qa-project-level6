import formatTimeAgo from '../../src/utils/formatTimeAgo.js'

describe('formatTimeAgo', () => {
    
    test('should return less than a minute ago when date is less than a minute ago', () => {
        const now = new Date();
        const fiveSecondsInSeconds = 5 * 1000;
        const FiveSecondsAgo = new Date(now.getTime() - fiveSecondsInSeconds)
        
        expect(formatTimeAgo(FiveSecondsAgo)).toBe('less than a minute ago');
    })

    test('should return x minutes ago when date is less than one hour ago', () => {
        const now = new Date();
        const fiveMinutesInSeconds = 5 * 60 * 1000;
        const fiveMinutesAgo = new Date(now.getTime() - fiveMinutesInSeconds)

        expect(formatTimeAgo(fiveMinutesAgo)).toContain('minutes ago');
    })

    test('should return x hours ago when date is less than one day ago', () => {
        const now = new Date();
        const fiveHoursInSeconds = 5 * 60 * 60 * 1000;
        const lessThanOneDayAgo = new Date(now.getTime() - fiveHoursInSeconds)

        expect(formatTimeAgo(lessThanOneDayAgo)).toContain('hours ago');
    })
    
    test('should return x days ago when date is less than one month ago', () => {
        const now = new Date();
        const fiveDaysInSeconds = 5 * 24 * 60 * 60 * 1000;
        const fiveDaysAgo = new Date(now.getTime() - fiveDaysInSeconds)

        expect(formatTimeAgo(fiveDaysAgo)).toContain('days ago');
    })
    
    test('should return x months ago when date is less than one year ago', () => {
        const now = new Date();
        const fiveMonthsInMonths = 5;
        const fiveMonthsAgo = now.setMonth(now.getMonth() - fiveMonthsInMonths)
        
        expect(formatTimeAgo(fiveMonthsAgo)).toContain('months ago');
    })
    
    test('should return x years ago when date is more than one year ago', () => {
        const now = new Date();
        const fiveYearsInYears = 5; 
        const fiveYearsAgo = now.setFullYear(now.getFullYear() - fiveYearsInYears)
        
        expect(formatTimeAgo(fiveYearsAgo)).toContain('years ago');
    })
})