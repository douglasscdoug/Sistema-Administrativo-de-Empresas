import { DateFormatPipe } from './date-format-pipe';

describe('DateFormatPipe', () => {
  let pipe: DateFormatPipe;

  beforeEach(() => {
    pipe = new DateFormatPipe();
  });

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should format a valid date string', () => {
    const result = pipe.transform('2023-10-01T12:00:00');
    expect(result).toBe('01/10/2023');
  });

  it('should return - for null value', () => {
    const result = pipe.transform(null);
    expect(result).toBe('-');
  });

  it('should return - for invalid date', () => {
    const result = pipe.transform('invalid-date');
    expect(result).toBe('-');
  });
});
